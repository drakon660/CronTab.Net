import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { authUser} from "../auth/authSlice";
import { getCron, selectCron } from "../auth/cronSlice";
import { Cron, User } from "../../app/api/interfaces";

const Login = () => {
    const dispatch = useAppDispatch();
    const cron:Cron[] = useAppSelector(selectCron);

    console.log("render");
    useEffect(() => {
       

    }, []);

    const loginOnClick = ()=>{

        let user:User = {
            "email":"drakon660@gmail,com",
          }
      
        dispatch(authUser(user));
    }

    const crontabOnClick = ()=>{
        dispatch(getCron())
    }

    return (
      <div>login
        <button onClick={loginOnClick}>login</button>
        <button onClick={crontabOnClick}>cron</button>
        <ul>
            {cron.map((item,index)=> <li key={index}>{item.cron} | {item.nextOccurence.toString()} | {item.task} </li>)}
        </ul>

    </div>)
}
export default Login;