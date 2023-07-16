import { useEffect } from "react";
import { useAppDispatch } from "../../app/hooks";
import { authUser } from "../auth/authSlice";

const Login = () => {

    const dispatch = useAppDispatch();


    useEffect(() => {

        dispatch(authUser())

    }, []);

    return (<div>login</div>)
}
export default Login;