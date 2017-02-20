import { NgModule }       from "@angular/core";
import { CommonModule }   from "@angular/common";
import { FormsModule } from "@angular/forms";

import { DatabaseListComponent }    from "./database-list.component";
import { DatabaseDetailComponent} from './database-detail.component';
import { DatabaseService } from "./database.service";
import { DatabaseRoutingModule } from "./database-routing.module";



@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        DatabaseRoutingModule
    ],
    declarations: [
        DatabaseListComponent,
        DatabaseDetailComponent
        
    ],
    providers: [
        DatabaseService
  
    ]
})
export class DatabasesModule { }
