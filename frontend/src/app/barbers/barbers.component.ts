import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { IBarber } from './barber';
import { BarbersService } from './barbers.service';
import { IVisit } from './visit';

@Component({
  selector: 'app-barbers',
  templateUrl: './barbers.component.html',
  styleUrls: ['./barbers.component.css']
})
export class BarbersComponent implements OnInit {
  barbersSub!: Subscription;
  visitsSub!: Subscription;
  barbers: IBarber[] = []
  visits: IVisit[] = []
  errorMessage = '';
  //public openDetails = false;

  constructor(
    private route: ActivatedRoute,
    private barbersService: BarbersService,
    private router: Router) 
    { }

  ngOnInit(): void {
    this.barbersSub = this.barbersService.getBarbers().subscribe({
      next: barbers => {
        this.barbers = barbers;
      },
      error: err => this.errorMessage = err
    });
    this.visitsSub = this.barbersService.getVisits().subscribe({
      next: visits => {
        this.visits = visits;
      },
      error: err => this.errorMessage = err
    });
  }
  onChooseBarber(barberId: number): void {
    console.log("barbers.component.ts -> onChooseBarber(" + barberId + ")" )
    this.router.navigate(['barbers', barberId, 'details']);
    //this.router.navigate(['/barbers/${barberId}/details']);
    //this.router.navigate(['/barbers/:barberId/details']);

    //  this.router.navigate(['hello/redirect/pageURL'],{queryParams:{id:1234,name:"ash"}});
    //this.openDetails = true;
  }
}
