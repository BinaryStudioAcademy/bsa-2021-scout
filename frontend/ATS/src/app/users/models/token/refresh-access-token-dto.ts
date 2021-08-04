import { AccessToken } from './access-token';

export interface RefreshAccessTokenDto {
  accessToken: AccessToken;
  refreshToken: string;
}
