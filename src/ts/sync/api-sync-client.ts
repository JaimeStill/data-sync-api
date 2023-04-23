import { IApiSyncClient } from './i-api-sync-client';
import { ISyncMessage } from './i-sync-message';
import { SyncAction } from './sync-action';
import { SyncClient } from './sync-client';

export abstract class ApiSyncClient<T>
    extends SyncClient<T>
    implements IApiSyncClient<T> {
    onAdd!: SyncAction;
    onUpdate!: SyncAction; 
    onRemove!: SyncAction;

    protected override initializeEvents(): void {
        super.initializeEvents();

        this.onAdd = new SyncAction("add", this.client);
        this.onUpdate = new SyncAction("update", this.client);
        this.onRemove = new SyncAction("remove", this.client);
    }

    constructor(public endpoint: string) {
        super(endpoint);
    }

    async add(message: ISyncMessage<T>) {
        await this.client.invoke('sendAdd', message);
    }

    async update(message: ISyncMessage<T>) {
        await this.client.invoke('sendUpdate', message);
    }

    async remove(message: ISyncMessage<T>) {
        await this.client.invoke('sendRemove', message);
    }
}