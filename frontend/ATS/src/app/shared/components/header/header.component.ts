import { Component, Input } from '@angular/core';
import _ from 'lodash';
import { AuthenticationService } from 'src/app/users/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent {
  public value: string = '';
  public dropdownOpened: boolean = false;
  @Input() removeButton = false;

  public constructor(
    private readonly service: AuthenticationService,
  ) {}

  public logout(): void {
    this.service.logout().subscribe(
      () => window.location.replace('/login'),
    );
  }
}
