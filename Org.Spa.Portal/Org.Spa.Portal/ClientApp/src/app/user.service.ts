import {RegistrationRequestModel, RegistrationResponseModel} from "./form/RegistrationRequestModel";
import {Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {environment} from '../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    private REGISTRAtION_ROUTE = environment.apiUrl + '/api/user';

    constructor(private httpClient: HttpClient) {
    }

    public registerUser(model: RegistrationRequestModel): Observable<RegistrationResponseModel> {
        return this.httpClient.post<RegistrationResponseModel>(this.REGISTRAtION_ROUTE, model, );
    }
}
