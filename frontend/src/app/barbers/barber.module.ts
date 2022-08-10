import { NgModule } from '@angular/core';
import { BarbersComponent } from './barbers.component';
import { RouterModule } from '@angular/router';
import { BarberDetailComponent } from './barber-detail.component';
import { BarberDetailGuard } from './barber-detail.guard';
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
  declarations: [
    /*BarbersComponent,
    BarberDetailComponent*/
  ],
  imports: [
    /*BrowserModule,
    RouterModule.forRoot([
      { path: 'barbers', component: BarbersComponent },
      { path: 'barbers/:id/details', canActivate: [BarberDetailGuard] , component: BarberDetailComponent }
    ]),*/
  ]
})
export class BarberModule { }
