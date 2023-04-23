import { SyncAction } from './sync-action';
import { ISyncClient } from './i-sync-client';
import { ISyncMessage } from './i-sync-message';

export interface IApiSyncClient<T> extends ISyncClient<T> {
    onAdd: SyncAction;
    onUpdate: SyncAction;
    onRemove: SyncAction;
    add: (message: ISyncMessage<T>) => Promise<void>;
    update: (message: ISyncMessage<T>) => Promise<void>;
    remove: (message: ISyncMessage<T>) => Promise<void>;    
}