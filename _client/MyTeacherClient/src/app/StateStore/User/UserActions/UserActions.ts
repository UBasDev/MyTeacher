import { createActionGroup, props } from "@ngrx/store"
import UserModel from "../../../Models/User/UserModel";

export const UserStateActions = createActionGroup({
    source: "Users",
    events: {
      "UserLoggedIn": props<{ loggedInUser: UserModel }>(),
      "UserLoggedOut": props<any>()
    },
  });