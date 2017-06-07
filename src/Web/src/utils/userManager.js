import { createUserManager } from "redux-oidc";

const userManagerConfig = {
  client_id: "web_app",
  redirect_uri: `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ""}/callback`,
  response_type: "token id_token",
  scope: "openid profile customer_service order_service catalog_service payment_service user_service",
  authority: `${window.location.protocol}//${window.location.hostname}:9999`,
  // silent_redirect_uri: `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ""}/silent_renew.html`,
  automaticSilentRenew: true,
  filterProtocolClaims: true,
  loadUserInfo: true
};

const userManager = createUserManager(userManagerConfig);

export default userManager;
