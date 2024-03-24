export class BaseResponse<T>{
    constructor(){
        this.isSuccessful = true
        this.serverTime = 0
        this.statusCode = 0
        this.traceId = ""
    }
    errorMessage?: string
    isSuccessful: boolean
    payload?: T
    serverTime: number
    statusCode: number
    traceId: string
}
export default BaseResponse