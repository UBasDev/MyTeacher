import BaseResponse from "../../../Helpers/BaseResponse/BaseResponse"

export class GetRolesWithoutAdminResponse extends BaseResponse<GetRolesWithoutAdminResponseModel[]>{
    
}
export class GetRolesWithoutAdminResponseModel{
    constructor(){
        this.name = ""
        this.shortCode = ""
    }
    id?: number
    name: string
    shortCode: string
}