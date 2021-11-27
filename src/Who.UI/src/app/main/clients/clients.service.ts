import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ClientsStore } from "./clients.store";
import { MClientCreateDto } from "./models/MClientCreateDto";
import { MClientDto } from "./models/MClientDto";
import { MClientListDto } from "./models/MClientListDto";
import { MClientUpdateDto } from "./models/MClientUpdateDto";

@Injectable({
    providedIn: 'any'
})
export class ClientsService {


    constructor(private http:HttpClient, private adminStore: ClientsStore) {

    }


    GetClients() {

        this.http.get<Array<MClientListDto>>("/api/admin/clients").subscribe(data => {
            this.adminStore.set(data)
        })
    }

    GetClient(id: string) {
        return this.http.get<MClientDto>(`/api/admin/clients/${id}`)
    }

    CreateClient(createModel: MClientCreateDto) {
        return this.http.post(`/api/admin/clients`, createModel);
    }

    UpdateClient(updateModel: MClientUpdateDto) {
        return this.http.put(`/api/admin/clients`, updateModel);
    }
    
}