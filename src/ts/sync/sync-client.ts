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
    protected channels: string[] = [];

    connected: boolean = false;
    state = (): HubConnectionState => this.client.state;

    onAdd!: SyncAction;
    onUpdate!: SyncAction;
    onSync!: SyncAction;
    onRemove!: SyncAction;

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

        this.onAdd = new SyncAction("add", this.client);
        this.onUpdate = new SyncAction("update", this.client);
        this.onSync = new SyncAction("sync", this.client);
        this.onRemove = new SyncAction("remove", this.client);
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

    async add(message: ISyncMessage<T>) {
        await this.client.invoke('sendCreate', message);
    }

    async update(message: ISyncMessage<T>) {
        await this.client.invoke('sendUpdate', message);
    }

    async sync(message: ISyncMessage<T>) {
        await this.client.invoke('sendSync', message);
    }

    async remove(message: ISyncMessage<T>) {
        await this.client.invoke('sendDelete', message);
    }
}