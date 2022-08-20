import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BarbersService } from '../barbers/barbers.service';
import { IBarberingService } from './barbering-services';
import { Pipe, PipeTransform } from '@angular/core';
import { IBarber } from '../barbers/barber';
import { MatDatepickerInputEvent, MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule  } from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSelectModule } from '@angular/material/select';
import { HttpClient, HttpHeaders, HttpErrorResponse } from "@angular/common/http";
import { catchError, Observable, throwError } from 'rxjs';
import { IClient } from '../clients/client';
import { IVisit } from '../barbers/visit';

@Pipe({ name: 'toInt' })
export class ToIntPipe implements PipeTransform {
  transform() {}
}
@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.css']
})
export class BookingComponent implements OnInit {
  clientsUrl = 'https://localhost:7071/api/clients';
  visitsUrl = 'https://localhost:7071/api/visits';
  errorMessage = '';
  barberingServices: IBarberingService[] | undefined; 
  hours: number[] | undefined
  client: IClient = {
    id: -1,
    name: ""
  }
  barber: IBarber = {
    id: -1,
    name: "",
    startHour: -1,
    endHour: -1
  }
  barberingService: IBarberingService = {
    id: -1,
    name: "",
    price: -1,
    duration: -1
  }
  visit: IVisit = {
    id: -1,
    barber: this.barber,
    barberingService: this.barberingService,
    date: "",
    hour: -1,
    client: this.client
  }
  
  barberingServiceChosen = false;
  dateChosen = false;
  hourChosen = false;
  clientSet = false;
  clickedConfirm = false;

  chosenBarberId = -1;
  chosenBarber: IBarber | undefined; 
  chosenBarberingServiceId = -1;
  chosenBarberingService: IBarberingService | undefined; 
  chosenDate = "1.01.2022"
  chosenHour = -1;
  clientName = "";

  visitResponse: any

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private barbersService: BarbersService,
    private http: HttpClient) { }

  ngOnInit(): void {
    const barberId = Number(this.route.snapshot.paramMap.get('id'));

      console.log("barber-detail.component.ts -> ngOnInit(" + barberId +")")
      if (barberId) {
        this.getBarberingServices(barberId);
      }
      this.chosenBarberId = barberId;

      this.hours = this.range(8,18);
  }
  
  getBarberById(barberId: number): void {
    console.log("booking.component.ts -> getBarberById(" + barberId +")")
    this.barbersService.getBarber(barberId).subscribe({
      next: barber => this.chosenBarber = barber,
      error: err => this.errorMessage = err
    });
  }
  
  getBarberingServiceById(barberId: number): void {
    console.log("booking.component.ts -> getBarberIngServiceById(" + barberId +")")
    this.barbersService.getBarberingService(barberId).subscribe({
      next: barberingService => this.chosenBarberingService = barberingService,
      error: err => this.errorMessage = err
    });
  }

  getBarberingServices(barberId: number): void {
    console.log("booking.component.ts -> getBarberingServices(" + barberId +")")
    this.barbersService.getBarberingServices(barberId).subscribe({
      next: barberingServices => this.barberingServices = barberingServices,
      error: err => this.errorMessage = err
    });
  }

  onChooseBarberingService(barberingServiceId: number): void {
    console.log("booking.component.ts -> onChooseBarberingService("+ barberingServiceId +")")
    this.chosenBarberingServiceId = barberingServiceId;
    this.barberingServiceChosen = true;
  }

  findBarberingServiceById(): IBarberingService | undefined 
  {
    var barberingService: IBarberingService | undefined;
    if (this.barberingServices != undefined) {
      console.log("chosenBarberingServiceId = " + this.chosenBarberingServiceId);
      barberingService = this.barberingServices.find(x => x.id == this.chosenBarberingServiceId);
      if (barberingService != undefined) {
        return barberingService;
      }
    }
    return undefined;
  }

  findBarberingServiceNameById(): string {
    var barberingService: IBarberingService | undefined;
    if (this.barberingServices != undefined) {
      //console.log("chosenBarberingServiceId = " + this.chosenBarberingServiceId);
      barberingService = this.barberingServices.find(x => x.id == this.chosenBarberingServiceId);
      if (barberingService != undefined) {
        return barberingService.name;
      }
    }
    return "ERROR: Could not find chosen service.";
  }

  setDate(event: any) {
    var date = event.value.toLocaleString();
    if (date.substring(9, 10) == ",") //np. 5.08.2022,
    {
      date = date.substring(0, 9);
    }
    else {//np. 11.08.2022
      date = date.substring(0, 10);
    }
    console.log(date);
    this.chosenDate = date;
    this.dateChosen = true;
  }

  setHour(event: any) {
    this.chosenHour = event.value;
    console.log("Hour: " + this.chosenHour);
    this.hourChosen = true;
  }

  setName(event:any) {
    this.clientName = event.target.value;
    console.log("Client name: " + this.clientName);
    this.clientSet = true;
  }

  async confirm()  {
    this.clickedConfirm = true;
    var client = this.client;
    client.id = 1;
    client.name = this.clientName;
    console.log("DONE; client.name = " + client.name);

    this.getBarberById(this.chosenBarberId);
    this.getBarberingServiceById(this.chosenBarberId);
    await new Promise(f => setTimeout(f, 1000));
 
    var visit = this.visit;
    visit.id = 1;
    visit.barber = this.chosenBarber;
    visit.barberingService = this.chosenBarberingService;
    visit.date = this.chosenDate;
    visit.hour = this.chosenHour;
    visit.client = client;
    
    if (visit.barber != undefined && visit.barberingService != undefined) {
      console.log("DONE;\nvisit.barber.name = " + visit.barber.name + 
                  "\nvisit.barberingService.name = " + visit.barberingService.name +
                  "\nvisit.date = " + visit.date +
                  "\nvisit.hour = " + visit.hour +
                  "\nvisit.client.name = " + visit.client.name);
    }
    this.createVisit(JSON.stringify(visit)).subscribe(visitResponse => {
      this.visitResponse = visitResponse
    });
    console.log("End Confirm.");

    this.router.navigate(['barbers']);
  }

  createVisit(visit: string): Observable<IVisit> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        Authorization: 'my-auth-token'
      })
    };
    console.log("Visit in Json: " + visit);
    return this.http.post<IVisit>(this.visitsUrl, visit, httpOptions)
    .pipe(catchError(this.handleError));
  }

  createClient(client: string): Observable<IClient> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        Authorization: 'my-auth-token'
      })
    };
    console.log("Client in Json: " + client);
    return this.http.post<IClient>(this.clientsUrl, client, httpOptions)
    .pipe(catchError(this.handleError));
  }

  range(min: number, max: number): number[] {
    var len = max - min + 1;
    var arr = new Array(len);
    for (var i=0; i<len; i++) {
      arr[i] = min + i;
    }
    return arr;
  }
  private handleError(err: HttpErrorResponse): Observable<never> {
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    console.error(errorMessage);
    return throwError(() => errorMessage);
  }
}
