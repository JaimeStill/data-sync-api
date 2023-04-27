export interface ISyncMessage<T> {
    readonly id: string;
    data: T;
    message: string;
}