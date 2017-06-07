const LOAD_ORDERS = "sc/order/LOAD_ORDERS";
const LOAD_ORDERS_SUCCESSED = "sc/order/LOAD_ORDERS_SUCCESSED";
const LOAD_ORDERS_FAILED = "sc/order/LOAD_ORDERS_FAILED";

const LOAD_ORDERS_URL = `http://localhost:8888/api/orders`;

const initialState = {
  loading: true,
  loaded: false,
  byIds: [],
  orders: {},
  error: null
};

export default function reducer(state = initialState, action = {}) {
  switch (action.type) {
    case LOAD_ORDERS:
      return {
        ...state,
        loading: true
      };

    case LOAD_ORDERS_SUCCESSED:
      const orders = action.orders.reduce((obj, order) => {
        obj[order.id] = order;
        return obj;
      }, {});
      return {
        ...state,
        byIds: action.orders.map(order => order.id),
        orders: orders,
        loaded: true,
        loading: false
      };

    case LOAD_ORDERS_FAILED:
      return {
        ...state,
        byIds: [],
        orders: {},
        error: action.error,
        loaded: true,
        loading: false
      };

    default:
      return state;
  }
}

export function ordersLoading() {
  return { type: LOAD_ORDERS };
}

export function loadOrders(orders) {
  return { type: LOAD_ORDERS_SUCCESSED, orders };
}

export function getOrders() {
  return (dispatch, getState) => {
    if (!getState()["orderStore"]["loaded"]) {
      dispatch(ordersLoading());
      return fetch(LOAD_ORDERS_URL)
        .then(response => response.json())
        .then(orders => dispatch(loadOrders(orders)));
    }
  };
}
