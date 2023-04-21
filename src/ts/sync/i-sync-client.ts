import { HubConnectionState } from '@microsoft/signalr';
import { SyncAction } from './sync-action';
import { ISyncMessage } from './i-sync-message';

export interface ISyncClient<T> {
    connected: boolean;
    endpoint: string;
    onAdd: SyncAction;
    onUpdate: SyncAction;
    onSync: SyncAction;
    onRemove: SyncAction;
    state: () => HubConnectionState;
    connect: () => Promise<void>;
    join: (name: string) => Promise<void>;
    leave: (name: string) => Promise<void>;
    add: (message: ISyncMessage<T>) => Promise<void>;
    update: (message: ISyncMessage<T>) => Promise<void>;
    sync: (message: ISyncMessage<T>) => Promise<void>;
    remove: (message: ISyncMessage<T>) => Promise<void>;
}