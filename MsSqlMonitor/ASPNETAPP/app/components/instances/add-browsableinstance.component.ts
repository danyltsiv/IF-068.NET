import { Component, OnInit, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { Location } from '@angular/common';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';

import { BrowsableInstance,Instance, Login, InstanceRole } from "./view-models";
import { InstanceAddUpdateDto } from "./dto-models";
import { InstanceService } from "./instance.service";
import { Buffer } from "./buffer";

@Component({
    moduleId: module.id,
    selector: 'browsableinstance-list',
    templateUrl: 'add-browsableinstance.component.html',
    styleUrls: ['add-browsableinstance.component.css'],
})
export class AddBrowsableInstanceComponent implements OnInit {

    instances: BrowsableInstance[];
    selectedInstance: Instance;
    newInstance: InstanceAddUpdateDto;

    @ViewChild('okModal') okModal: string;
    message: string;


    constructor(
        private router: Router,
        private instanceService: InstanceService,
        private buffer: Buffer,
        private modalService: NgbModal,
        private location: Location) { }

    ngOnInit(): void {


        this.message = "";

        this.getBrowsableInstances();
    }



    setWindowsAuth()
    {
        this.newInstance.isWindowsAuthentication = true;
    }

    setSQLAuth() {
        this.newInstance.isWindowsAuthentication = false;
    }


    getBrowsableInstances(): void
    {
  
    this.instanceService.getBrowsableInstances()
            .then(instances => {              
                this.instances = instances;    
             })
            .catch(error => {
                console.error("instancel list component: getInstances");
                console.error(error);
            }
            );

    }

    openModal(content: any, selectedInstance: Instance): void {
        this.selectedInstance = selectedInstance;
        this.modalService.open(content, { size: 'lg' })
            .result.then(() => {
                this.selectedInstance = null;
            });
    }
    


    openAddInstanceModal(content: HTMLElement,serverName: string, instanceName: string) {

        this.newInstance = new InstanceAddUpdateDto();
        this.newInstance.instanceName = instanceName;
        this.newInstance.serverName = serverName;

        this.modalService.open(content, { size: 'sm' })
            .result.then(result => {
                this.instanceService.addInstance(this.newInstance)
                    .then(addedInstance => {
                       // if (addedInstance != null)
                       //     this.instances.push(addedInstance);
                    });
            })
            .catch(reason => console.info('Instance add cancelled'));
    }

    showModalMessage(msg:string) {

        this.message = msg;

        this.modalService.open(this.okModal, { windowClass: 'OkModal' })
            .result.then(result => {
            
            });
    }

    checkConnection(instance: InstanceAddUpdateDto): void {

        //alert("You are checking instance connection: " + instance);

        this.instanceService.checkConnection(instance)
            .then(result => {

              //  alert(result);
               this.showModalMessage(result);
          
            })
            .catch(error => {

                console.error("Can't connect!");
                console.error(error);
               // alert("Can't connect!\n"+error);
            }
            );


    }




}