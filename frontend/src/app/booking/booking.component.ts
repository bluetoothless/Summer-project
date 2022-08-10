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
  
  barberingServiceChosen = false;
  dateChosen = false;
  hourChosen = false;
  clientSet = false;

  chosenBarberId = -1;
  chosenBarberingServiceId = -1;
  chosenDate = "1.01.2022"
  chosenHour = -1;
  clientName = "";

  clientResponse: any

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

  confirm()  {var client = this.client;
    this.client.id = 1;
    client.name = this.clientName;
    console.log("DONE; client.name = " + client.name);

    this.createClient(JSON.stringify(client)).subscribe(clientResponse => {
      this.clientResponse = clientResponse
    });
    console.log("End Confirm.");
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
  /*addReview(reviewForm: IVisitReviewForm): Observable<void> {
    return this.http.post<void>('/api/reviews', reviewForm);
  }

  addReviewReply(reviewId: string, reviewReplyForm: IVisitReviewReplyForm): Observable<any> {
    return this.http.post<void>(`/api/reviews/${reviewId}/replies`, reviewReplyForm);
  }*/

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
