import { RefreshAccessTokenDto } from '../token/refresh-access-token-dto';
import { User } from '../user';

export interface AuthUser {
  user: User;
  token: RefreshAccessTokenDto;
}
