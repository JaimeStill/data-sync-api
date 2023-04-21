import { ActionType } from './action-type';

export interface ISyncMessage<T> {
    readonly id: string;
    data: T;
    action: ActionType;
    channel: string;
    message: string;
}