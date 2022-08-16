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
  errorMessage = 'errorrrr';
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
        visits.sort((a, b) => stringToDate(a.date, a.hour) > stringToDate(b.date, a.hour) ? 1 : -1);
      },
      error: err => this.errorMessage = err
    });
    console.log(this.barbers[0].name);
    if (this.visits[0].barberingService != undefined) {
      console.log(this.visits[0].barberingService.name);
    }

    function stringToDate(s: string, hour: number) : Date {
      const[day, month, year] = s.split('.');
      var a = new Date(+year, +month - 1, +day, +hour);
      return new Date(+year, +month - 1, +day);
    }
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
