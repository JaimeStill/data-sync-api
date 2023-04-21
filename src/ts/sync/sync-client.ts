import {
    HubConnection,
    HubConnectionBuilder,
    LogLevel
} from '@microsoft/signalr';

import { ISyncClient } from './i-sync-client';
import { ISyncMessage } from './i-sync-message';
import { SyncAction } from './sync-action';

export abstract class SyncClient implements ISyncClient {
    protected client!: HubConnection;
    protected channels: string[] = [];

    connected: boolean = false;
    state = (): string => this.client.state;

    onCreate!: SyncAction;
    onUpdate!: SyncAction;
    onSync!: SyncAction;
    onDelete!: SyncAction;

    protected buildHubConnection = (endpoint: string): HubConnection =>
        new HubConnectionBuilder()
            .withUrl(endpoint, {
                withCredentials: true
            })
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();

    protected initializeEvents() {
        this.client.onclose(async () => {
            this.connected = false;
            await this.connect();
        });

        this.onCreate = new SyncAction("Create", this.client);
        this.onUpdate = new SyncAction("Update", this.client);
        this.onSync = new SyncAction("Sync", this.client);
        this.onDelete = new SyncAction("Delete", this.client);
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

    async join(name: string) {
        if (!this.channels.includes(name)) {
            await this.client.invoke('join', name);
            this.channels.push(name);
        }
    }

    async leave(name: string) {
        if (this.channels.includes(name)) {
            await this.client.invoke('leave', name);
            this.channels = this.channels.filter(c => c !== name);
        }
    }

    async create<T>(message: ISyncMessage<T>) {
        await this.client.invoke('sendCreate', message);
    }

    async update<T>(message: ISyncMessage<T>) {
        await this.client.invoke('sendUpdate', message);
    }

    async sync<T>(message: ISyncMessage<T>) {
        await this.client.invoke('sendSync', message);
    }

    async delete<T>(message: ISyncMessage<T>) {
        await this.client.invoke('sendDelete', message);
    }
}