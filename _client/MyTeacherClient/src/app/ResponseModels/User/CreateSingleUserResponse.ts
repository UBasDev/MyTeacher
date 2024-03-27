import BaseErrorResponse from "../../Helpers/BaseResponse/BaseErrorResponse";
import BaseResponse from "../../Helpers/BaseResponse/BaseResponse";

export class CreateSingleUserResponse extends BaseResponse<CreateSingleUserResponseModel>{

}
export class ErrorCreateSingleUserResponse extends BaseErrorResponse<CreateSingleUserResponseModel>{
    
}
export class CreateSingleUserResponseModel{
    constructor(){
        this.username = ""
        this.email = ""
        this.roleName = ""
        this.firstname = ""
        this.lastname = ""
        this.age = 0
        this.birthDate = 0
    }
    username: string
    email: string
    roleName: string
    firstname: string
    lastname: string
    age: number
    birthDate: number
}