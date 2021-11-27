import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { FirstSetupModel } from './models/first-setup-model';
import { FirstSetupViewModel } from './models/first-setup-view-model';

@Injectable()
export class FirstSetupService {

    constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute) {
    }
    
    public GetSetupViewModel() {
        return this.http.get<FirstSetupViewModel>(`/api/first-setup`);
    }

    public SubmitFirstSetupViewModel(model: FirstSetupModel) {
        return this.http.post(`/api/first-setup`, model);
    }

}
