import { Component, OnInit } from '@angular/core';
import { ConfirmEmailDto } from '../../models/confirm-email-dto';
import { AuthenticationService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss'],
})
export class ConfirmEmailComponent implements OnInit {

  constructor(public authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private notificationService: NotificationService,
    private router: Router) { }
    
  IsEmailConfirmed : boolean  = false;

  ngOnInit(): void {
    const dto : ConfirmEmailDto =  {
      email : this.route.snapshot.queryParamMap.get('email') as string,
      token : this.route.snapshot.queryParamMap.get('token') as string,
    };
    this.authenticationService.confirmEmail(dto).pipe()
      .subscribe(() => {
        this.IsEmailConfirmed = true;
      },
      () => { 
        this.notificationService.showErrorMessage('Something went wrong');
      });
  }

}
