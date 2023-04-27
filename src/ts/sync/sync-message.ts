import { v4 as uuid } from 'uuid';
import { ISyncMessage } from './i-sync-message';

export class SyncMessage<T> implements ISyncMessage<T> {
    readonly id: string;

    constructor(
        public data: T,
        public message: string = ''
    ) {
        this.id = uuid();
    }
}