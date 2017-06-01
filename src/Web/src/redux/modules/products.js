// import apiRequest from "../../utils/request";

// Actions
const LOAD_PRODUCTS = "shoppingcart/products/LOAD_PRODUCTS";
const LOAD_PRODUCTS_SUCCESSED = "shoppingcart/products/LOAD_PRODUCTS_SUCCESSED";
const LOAD_PRODUCTS_FAILED = "shoppingcart/products/LOAD_PRODUCTS_FAILED";

/*const initialState = {
  loaded: false,
  error: null
};*/

// Reducer
export default function reducer(state = {}, action = {}) {
  switch (action.type) {
    case LOAD_PRODUCTS:
      return {
        ...state,
        loading: true
      };

    case LOAD_PRODUCTS_SUCCESSED:
      return {
        ...state,
        products: action.products,
        loaded: true,
        loading: false
      };

    case LOAD_PRODUCTS_FAILED:
      return {
        ...state,
        error: action.error,
        loaded: true,
        loading: false
      };

    default:
      return state;
  }
}

export function productsLoading() {
  return { type: LOAD_PRODUCTS };
}

// Action Creators
export function loadProducts(products) {
  return { type: LOAD_PRODUCTS_SUCCESSED, products };
}

export function getProducts() {
  // return dispatch => apiRequest.apiRequest(`http://localhost:8888/api/products`);
  return dispatch => {
    dispatch(productsLoading());

    return fetch(`http://localhost:8888/api/products`)
      .then(response => response.json())
      .then(products => dispatch(loadProducts(products)));
  };
}
