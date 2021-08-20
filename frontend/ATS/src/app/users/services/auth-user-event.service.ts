import { Injectable } from '@angular/core';

import { Subject } from 'rxjs';
import { User } from '../models/user';

@Injectable({ providedIn: 'root' })
export class AuthUserEventService {
  private onUserChanged = new Subject<User | null>();
  public userChangedEvent$ = this.onUserChanged.asObservable();

  public userChanged(user: User | null) {
    this.onUserChanged.next(user);
  }
}
