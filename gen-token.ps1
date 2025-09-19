param(
  [Parameter(Mandatory=$true)]
  [string]$ClaimType,                  # e.g. "role" or a URI claim type
  [Parameter(Mandatory=$true)]
  [string]$ClaimValue,                 # e.g. "admin"

  [string]$Issuer = "https://issuer.example.com/",
  [string]$Audience = "api://your-api",
  [int]$ExpiresInMinutes = 60,

  [hashtable]$AdditionalClaims,        # e.g. @{ "name"="Alice"; "scope"="read:all" }

  [ValidateSet("HS256","RS256")]
  [string]$Algorithm = "HS256",

  # HS256
  [string]$Secret,                     # required if -Algorithm HS256

  # RS256
  [string]$PfxPath,                    # required if -Algorithm RS256
  [SecureString]$PfxPassword,          # required if -Algorithm RS256
  [string]$Kid                         # optional key id for header
)

function ConvertTo-Base64Url([byte[]]$bytes) {
  $b64 = [Convert]::ToBase64String($bytes)
  return $b64.TrimEnd('=').Replace('+','-').Replace('/','_')
}

function Get-Epoch([datetime]$dt) {
  return [DateTimeOffset]::new($dt.ToUniversalTime()).ToUnixTimeSeconds()
}

# --- Build header ---
$header = @{
  typ = "JWT"
  alg = $Algorithm
}
if ($Kid) { $header.kid = $Kid }

# --- Build payload ---
$now = [DateTime]::UtcNow
$iat = Get-Epoch $now
$nbf = $iat
$exp = Get-Epoch ($now.AddMinutes($ExpiresInMinutes))

$payload = @{
  iat = [int64]$iat
  nbf = [int64]$nbf
  exp = [int64]$exp
}

if ($Issuer)  { $payload.iss = $Issuer }
if ($Audience){ $payload.aud = $Audience }

# set the specific claim type/value requested
$payload[$ClaimType] = $ClaimValue

# merge any extra claims
if ($AdditionalClaims) {
  foreach ($k in $AdditionalClaims.Keys) {
    $payload[$k] = $AdditionalClaims[$k]
  }
}

# --- Serialize parts (compact, no whitespace) ---
$headerJson  = ($header | ConvertTo-Json -Compress -Depth 10)
$payloadJson = ($payload | ConvertTo-Json -Compress -Depth 10)

$encHeader  = ConvertTo-Base64Url ([Text.Encoding]::UTF8.GetBytes($headerJson))
$encPayload = ConvertTo-Base64Url ([Text.Encoding]::UTF8.GetBytes($payloadJson))
$toSign     = "$encHeader.$encPayload"

# --- Sign ---
switch ($Algorithm) {
  "HS256" {
    # If no secret provided, generate a default 256-bit (32 bytes) random secret
    if (-not $Secret -or [string]::IsNullOrWhiteSpace($Secret)) {
      $rand = New-Object byte[] 32
      [System.Security.Cryptography.RandomNumberGenerator]::Fill($rand)
      # store as Base64Url string so it can be easily reused via -Secret next time
      $Secret = ConvertTo-Base64Url $rand
      Write-Verbose "Generated default 256-bit secret (Base64Url). Use -Verbose to display." -Verbose:$false
    }
    $hmac = [System.Security.Cryptography.HMACSHA256]::new([Text.Encoding]::UTF8.GetBytes($Secret))
    $sigBytes = $hmac.ComputeHash([Text.Encoding]::UTF8.GetBytes($toSign))
    $encSig = ConvertTo-Base64Url $sigBytes
  }
  "RS256" {
    if (-not $PfxPath -or -not $PfxPassword) { throw "For RS256, provide -PfxPath and -PfxPassword." }
    $cert = [System.Security.Cryptography.X509Certificates.X509Certificate2]::new()
    $pwd  = [Runtime.InteropServices.Marshal]::PtrToStringAuto(
              [Runtime.InteropServices.Marshal]::SecureStringToBSTR($PfxPassword))
    $flags = [System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]::Exportable
    $cert.Import($PfxPath, $pwd, $flags)
    $rsa = $cert.GetRSAPrivateKey()
    if (-not $rsa) { throw "No RSA private key found in PFX." }
    $sigBytes = $rsa.SignData([Text.Encoding]::UTF8.GetBytes($toSign),
                              [System.Security.Cryptography.HashAlgorithmName]::SHA256,
                              [System.Security.Cryptography.RSASignaturePadding]::Pkcs1)
    $encSig = ConvertTo-Base64Url $sigBytes
  }
}

# --- Output token ---
$token = "$toSign.$encSig"
Write $token
