const LOAD_PAYMENTS = "sc/payment/LOAD_PAYMENTS";
const LOAD_PAYMENTS_SUCCESSED = "sc/payment/LOAD_PAYMENTS_SUCCESSED";
const LOAD_PAYMENTS_FAILED = "sc/payment/LOAD_PAYMENTS_FAILED";

const LOAD_PAYMENTS_URL = `http://localhost:8888/api/payments`;
const CALLBACK_FROM_PAYMENT_GATEWAY_URL = `http://localhost:8888/api/payments`;

const initialState = {
  loading: true,
  loaded: false,
  byIds: [],
  payments: {},
  error: null
};

export default function reducer(state = initialState, action = {}) {
  switch (action.type) {
    case LOAD_PAYMENTS:
      return {
        ...state,
        loading: true
      };

    case LOAD_PAYMENTS_SUCCESSED:
      const payments = action.payments.reduce((obj, payment) => {
        obj[payment.id] = payment;
        return obj;
      }, {});
      return {
        ...state,
        byIds: action.payments.map(payment => payment.id),
        payments: payments,
        loaded: true,
        loading: false
      };

    case LOAD_PAYMENTS_FAILED:
      return {
        ...state,
        byIds: [],
        payments: {},
        error: action.error,
        loaded: true,
        loading: false
      };

    default:
      return state;
  }
}

export function callbackFromPaymentGateway(paymentId) {
  return dispatch => {
    return fetch(
      CALLBACK_FROM_PAYMENT_GATEWAY_URL +
        `/${paymentId}/payment-gateway-callback`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        }
      }
    )
      .then(response => response.json())
      .then(products => dispatch(getPayments()));
  };
}

export function paymentsLoading() {
  return { type: LOAD_PAYMENTS };
}

export function loadPayments(payments) {
  return { type: LOAD_PAYMENTS_SUCCESSED, payments };
}

export function getPayments() {
  return (dispatch, getState) => {
    if (!getState()["paymentStore"]["loaded"]) {
      dispatch(paymentsLoading());
      return fetch(LOAD_PAYMENTS_URL)
        .then(response => response.json())
        .then(payments => dispatch(loadPayments(payments)));
    }
  };
}
