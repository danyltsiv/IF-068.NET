﻿import { Request, XHRBackend, BrowserXhr, ResponseOptions, XSRFStrategy, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

export class AuthenticationConnectionBackend extends XHRBackend {

    constructor(_browserXhr: BrowserXhr, _baseResponseOptions: ResponseOptions, _xsrfStrategy: XSRFStrategy) {
        super(_browserXhr, _baseResponseOptions, _xsrfStrategy);
    }

    createConnection(request: Request) {
        let xhrConnection = super.createConnection(request);
        xhrConnection.response = xhrConnection.response.catch((error: Response) => {
            if (error.status === 401 || error.status === 403) {
                localStorage.clear();
                window.location.href = 'https://localhost:44301';
            }
            return Observable.throw(error);
        });
        return xhrConnection;
    }

}