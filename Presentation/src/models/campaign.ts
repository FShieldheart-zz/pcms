import { Product } from './product';

export class Campaign {
    id: number;
    name: string;
    start_date: Date;
    end_date: Date;
    product: Product;
    product_id: number;
    is_active: boolean;
}
