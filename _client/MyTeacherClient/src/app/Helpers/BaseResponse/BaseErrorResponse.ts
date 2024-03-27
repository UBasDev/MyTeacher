export class BaseErrorResponse<T>{
    constructor(){
        this.error = new BaseErrorResponseModel<T>()
        this.message = ""
        this.name = ""
        this.ok = false
        this.status = 0
        this.statusText = ""
        this.url = ""
    }
    error: BaseErrorResponseModel<T>
    message: string
    name: string
    ok: boolean
    status: number
    statusText: string
    url: string
}
class BaseErrorResponseModel<T>{
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
export default BaseErrorResponse