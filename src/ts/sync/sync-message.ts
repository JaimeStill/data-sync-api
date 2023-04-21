import { v4 as uuid } from 'uuid';
import { ActionType } from './action-type';
import { ISyncMessage } from './i-sync-message';

export class SyncMessage<T> implements ISyncMessage<T> {
    readonly id: string;

    constructor(
        public channel: string,
        public data: T,
        public action: ActionType,
        public message: string = ''
    ) {
        this.id = uuid();
    }
}