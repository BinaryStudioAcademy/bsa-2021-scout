import { EntityType } from 'src/app/shared/enums/entity-type.enum';
import { User } from 'src/app/users/models/user';

export interface ArchivedEntity {
  id: string;
  entityId: string;
  entityType: EntityType;
  expirationDate: Date;
  userId: string;
  user: User;
}