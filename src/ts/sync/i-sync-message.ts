import { ActionType } from './action-type';

export interface ISyncMessage<T> {
    readonly id: string;
    channel: string;
    data: T;
    action: ActionType;
    message: string;
}