import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from "@angular/common/http";
import { IBarber } from './barber';
import { IVisit } from './visit';
import { catchError, tap, map, Observable, throwError } from 'rxjs';
import { IBarberingService } from '../booking/barbering-services';

@Injectable({
  providedIn: 'root'
})
export class BarbersService {
  private barbersUrl = 'https://localhost:7071/api/barbers';
  private visitsUrl = 'https://localhost:7071/api/visits';
  
  constructor(private http: HttpClient) { }

  getBarbers(): Observable<IBarber[]> {
    console.log("barbers.service -> getBarbers()");
    return this.http.get<IBarber[]>(this.barbersUrl)
      .pipe(
        /*tap(data => console.log('All: ', JSON.stringify(data))),*/
        catchError(this.handleError)
      );
  }

  getBarber(id: number): Observable<IBarber | undefined> {
    console.log("barbers.service -> getBarber(" + id +")");
    return this.getBarbers()
      .pipe(
        map((barbers: IBarber[]) => barbers.find(b => b.id === id))
      );
  }

  getBarberingService(id: number): Observable<IBarberingService | undefined> {
    console.log("barbers.service -> getBarberingService(" + id +")");
    return this.getBarberingServices(id)
      .pipe(
        map((barberingServices: IBarberingService[]) => barberingServices.find(b => b.id === id))
      );
  }

  getBarberingServices(id: number): Observable<IBarberingService[]> {
    console.log("barbers.service -> getBarberingServices("+ id +")");
    var barberingServicesUrl = this.barbersUrl + '/' + id.toString() + '/booking';
    return this.http.get<IBarberingService[]>(barberingServicesUrl)
      .pipe(
        /*tap(data => console.log('All: ', JSON.stringify(data))),*/
        catchError(this.handleError)
      );
  }

  getVisits(): Observable<IVisit[]> {
    console.log("barbers.service -> getVisits()");
    return this.http.get<IVisit[]>(this.visitsUrl)
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(err: HttpErrorResponse): Observable<never> {
    // in a real world app, we may send the server to some remote logging infrastructure
    // instead of just logging it to the console
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    console.error(errorMessage);
    return throwError(() => errorMessage);
  }
}
