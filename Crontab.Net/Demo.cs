namespace Crontab.Net;

public static class Demo
{
    public static string Data = "* * * * * sh run something " +
                                "\n*/5 * * * * sh run something2 " +
                                "\n0 2 * * 1-5 python /path/to/my/script.py " +
                                "\n0 */2 * * * sh /path/to/my/script.sh " +
                                "\n30 4,16 * * * perl /path/to/my/script.pl " +
                                "\n0 0 * * 0 tar -czvf /path/to/backup.tar.gz /path/to/files/ " +
                                "\n*/15 * * * * python3 /path/to/my/script.py arg1 arg2 " +
                                "\n0 0 1 * * php /path/to/my/script.php " +
                                "\n*/10 * * * * sh /path/to/my/script.sh >> /var/log/mylog.log " +
                                "\n0 8,16 * * 1-5 /usr/bin/python3 /path/to/my/script.py >> /var/log/mylog.log 2>&1";
}