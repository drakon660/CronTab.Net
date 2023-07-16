import axios from "axios";

axios.defaults.baseURL = "https://localhost:7269";

export const authUser = async() =>
{
    const { data }  = await axios.post<string>('/user', {
        headers: {
          'Content-Type': 'application/json'
        }
      })

    return data;
}