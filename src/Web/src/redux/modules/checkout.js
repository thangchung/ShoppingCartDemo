const LOAD_DEFAULT_DATA = "sc/checkout/LOAD_DEFAULT_DATA";
const DO_CHECKOUT_SUCCEED = "sc/checkout/DO_CHECKOUT_SUCCEED";
const DO_CHECKOUT_FAILED = "sc/checkout/DO_CHECKOUT_FAILED";

const PROCESS_CHECKOUT_URL = `http://localhost:8888/api/checkout`;

const initialState = {
  loaded: false,
  shipInfo: {},
  error: null
};

const shipInfo = {
  name: "Ship information",
  address: "123 Address ABC",
  city: "Ho Chi Minh",
  region: "Tan Binh district",
  postalCode: "7000",
  country: "Vietnam"
};

export default function reducer(state = initialState, action = {}) {
  switch (action.type) {
    case LOAD_DEFAULT_DATA:
      return {
        ...state,
        shipInfo: shipInfo
      };

    case DO_CHECKOUT_SUCCEED:
      return state;

    case DO_CHECKOUT_FAILED:
      return state;

    default:
      return state;
  }
}

export function doCheckoutSucceed() {
  return { type: DO_CHECKOUT_SUCCEED };
}

export function doCheckoutFailed(error) {
  console.log(error);
  return { type: DO_CHECKOUT_FAILED };
}

export function loadDefaultData() {
  return { type: LOAD_DEFAULT_DATA };
}

export function fetchDefaultData() {
  return dispatch => {
    return dispatch(loadDefaultData());
  };
}

export function doCheckout(cart, shipInfo) {
  var products = [];
  cart.forEach(function(c) {
    const { product, quantity, ...remains } = c;
    products.push({
      productId: product.id,
      quantity
    });
  }, this);

  const payload = {
    products: products,
    shipInfo: shipInfo
  };

  return (dispatch, getState) => {
    return fetch(PROCESS_CHECKOUT_URL, {
      method: "POST",
      body: JSON.stringify(payload),
      headers: {
        "Content-Type": "application/json"
      }
    })
      .then(response => response.json())
      .then(
        products => {
          getState().orderStore.loading = true;
          getState().orderStore.loaded = false;
          getState().paymentStore.loading = true;
          getState().paymentStore.loaded = false;
          getState().auditStore.loading = true;
          getState().auditStore.loaded = false;
          getState().homeStore.cart = [];
          return dispatch(doCheckoutSucceed());
        },
        error => dispatch(doCheckoutFailed(error))
      );
  };
}
