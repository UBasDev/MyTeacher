import { createReducer, on } from "@ngrx/store";
import { UserStateActions } from "../UserActions/UserActions";
import UserModel from "../../../Models/User/UserModel";

let userDataFromLocalStorage: UserModel | null = new UserModel()

try{
    userDataFromLocalStorage = localStorage.getItem("userProfile") ? JSON.parse(localStorage.getItem("userProfile") ?? "") : null
}catch(ex){

}

export interface IUserInitialState{
    isLoggedIn: boolean
    username: string
    email: string
    roleName: string
    firstname: string
    lastname: string
    age: number
    birthDate: number
}
export const UserInitialState: IUserInitialState = {
    isLoggedIn: userDataFromLocalStorage != null,
    username: userDataFromLocalStorage?.username ?? "",
    email: userDataFromLocalStorage?.email ?? "",
    roleName: userDataFromLocalStorage?.roleName ?? "",
    firstname: userDataFromLocalStorage?.firstname ?? "",
    lastname: userDataFromLocalStorage?.lastname ?? "",
    age: userDataFromLocalStorage?.age ?? 0,
    birthDate : userDataFromLocalStorage?.birthDate ?? 0
}

export const GlobalUserReducer = createReducer(
    UserInitialState,
    on(UserStateActions.userLoggedIn, (_state, payload)=>{
        return {
            ..._state,
            isLoggedIn: true,
            username: payload.loggedInUser.username,
            firstname: payload.loggedInUser.firstname,
            lastname: payload.loggedInUser.lastname,
            email: payload.loggedInUser.email,
            age: payload.loggedInUser.age,
            roleName: payload.loggedInUser.roleName,
            birthDate: payload.loggedInUser.birthDate
        }
    })
)