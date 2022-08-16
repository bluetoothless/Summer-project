import { IBarberingService } from "../booking/barbering-services";
import { IClient } from "../clients/client";
import { IBarber } from "./barber";

export interface IVisit {
    id: number;
    barber: IBarber | undefined;
    barberingService: IBarberingService | undefined;
    date: string;
    hour: number;
    client: IClient;
}