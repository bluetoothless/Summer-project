import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BarbersComponent } from './barbers/barbers.component';
import { RouterModule } from '@angular/router';
import { BarberModule } from './barbers/barber.module';
import { BarberDetailComponent } from './barbers/barber-detail.component';
import { BarberDetailGuard } from './barbers/barber-detail.guard';
import { BookingComponent } from './booking/booking.component';
import { ToIntPipe } from './to-int-pipe';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule  } from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSelectModule } from '@angular/material/select';

@NgModule({
  declarations: [
    AppComponent,
    BarbersComponent,
    BarberDetailComponent,
    BookingComponent,
    ToIntPipe
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      { path: 'barbers', component: BarbersComponent },
      { path: 'barbers/:id/details', component: BarberDetailComponent },
      //{ path: 'barbers/:id/booking', component: BookingComponent },
      { path: '', redirectTo: 'barbers', pathMatch: 'full' },
      //{ path: '**', redirectTo: 'barbers', pathMatch: 'full' },
    ]),
    RouterModule.forChild([
      { path: 'barbers/:id/booking', component: BookingComponent }
    ]),
    //BarberModule
  ],
  providers: [{
    provide: LOCALE_ID,
    useValue: 'pl'
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
