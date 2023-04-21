import { HubConnectionState } from '@microsoft/signalr';
import { SyncAction } from './sync-action';
import { ISyncMessage } from './i-sync-message';

export interface ISyncClient {
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
    add: <T>(message: ISyncMessage<T>) => Promise<void>;
    update: <T>(message: ISyncMessage<T>) => Promise<void>;
    sync: <T>(message: ISyncMessage<T>) => Promise<void>;
    remove: <T>(message: ISyncMessage<T>) => Promise<void>;
}