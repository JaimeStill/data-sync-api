import { SyncAction } from './sync-action';
import { ISyncMessage } from './i-sync-message';

export interface ISyncClient {
    connected: boolean;
    endpoint: string;
    onCreate: SyncAction;
    onUpdate: SyncAction;
    onSync: SyncAction;
    onDelete: SyncAction;
    state: () => string;
    connect: () => Promise<void>;
    join: (name: string) => Promise<void>;
    leave: (name: string) => Promise<void>;
    create: <T>(message: ISyncMessage<T>) => Promise<void>;
    update: <T>(message: ISyncMessage<T>) => Promise<void>;
    sync: <T>(message: ISyncMessage<T>) => Promise<void>;
    delete: <T>(message: ISyncMessage<T>) => Promise<void>;
}