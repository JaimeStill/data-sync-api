import {
    HubConnection,
    HubConnectionBuilder,
    HubConnectionState,
    LogLevel
} from '@microsoft/signalr';

import { ISyncClient } from './i-sync-client';
import { ISyncMessage } from './i-sync-message';
import { SyncAction } from './sync-action';

export abstract class SyncClient<T> implements ISyncClient<T> {
    protected client!: HubConnection;

    connected: boolean = false;
    state = (): HubConnectionState => this.client.state;

    onSync!: SyncAction;

    protected buildHubConnection = (endpoint: string): HubConnection =>
        new HubConnectionBuilder()
            .withUrl(endpoint, {
                withCredentials: true
            })
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();

    protected initializeEvents(): void {
        this.client.onclose(async () => {
            this.connected = false;
            await this.connect();
        });

        this.onSync = new SyncAction("sync", this.client);
    }

    constructor(public endpoint: string) {
        this.client = this.buildHubConnection(endpoint);
        this.initializeEvents();
    }

    async connect() {
        try {
            await this.client.start();
        } catch (err) {
            setTimeout(this.connect, 5000);
        }
    }

    async sync(message: ISyncMessage<T>) {
        await this.client.invoke('sendSync', message);
    }
}