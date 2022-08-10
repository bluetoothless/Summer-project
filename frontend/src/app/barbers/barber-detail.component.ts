import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IBarber } from './barber';
import { BarbersService } from './barbers.service';
@Component({
  selector: 'app-barber-detail',
  templateUrl: './barber-detail.component.html',
  styleUrls: ['./barber-detail.component.css']
})
export class BarberDetailComponent implements OnInit {
  errorMessage = '';
  barber: IBarber | undefined; 
  showBookingComponent = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private barbersService: BarbersService) 
    { 
    }

  ngOnInit(): void {
      const id = Number(this.route.snapshot.paramMap.get('id'));

      console.log("barber-detail.component.ts -> ngOnInit(" + id +")")

      if (id) {
        this.getBarber(id);
      }
  }

  getBarber(barberId: number): void {
    console.log("barber-detail.component.ts -> getBarber(" + barberId +")")
    this.barbersService.getBarber(barberId).subscribe({
      next: barber => this.barber = barber,
      error: err => this.errorMessage = err
    });
  }

  onChooseBooking(barberId: number): void {
    console.log("barber-detail.component.ts -> onChooseBooking(" + barberId +")")
    this.showBookingComponent = true;
  }

  getShowBookingComponent():boolean {
    return this.showBookingComponent;
  }

  onBack(): void {
    console.log("barber-detail.component.ts -> onBack()")
    this.router.navigate(['/barbers']);
  }
}
