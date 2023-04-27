import { HubConnectionState } from '@microsoft/signalr';
import { SyncAction } from './sync-action';
import { ISyncMessage } from './i-sync-message';

export interface ISyncClient<T> {
    connected: boolean;
    endpoint: string;
    onSync: SyncAction;
    state: () => HubConnectionState;
    connect: () => Promise<void>;
    sync: (message: ISyncMessage<T>) => Promise<void>;
}