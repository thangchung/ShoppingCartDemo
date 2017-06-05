const LOAD_AUDITS = "sc/audit/LOAD_AUDITS";
const LOAD_AUDITS_SUCCESSED = "sc/audit/LOAD_AUDITS_SUCCESSED";
const LOAD_AUDITS_FAILED = "sc/audit/LOAD_AUDITS_FAILED";

const LOAD_AUDITS_URL = `http://localhost:8888/api/audits`;

const initialState = {
  loading: true,
  loaded: false,
  byIds: [],
  audits: {},
  error: null
};

export default function reducer(state = initialState, action = {}) {
  switch (action.type) {
    case LOAD_AUDITS:
      return {
        ...state,
        loading: true
      };

    case LOAD_AUDITS_SUCCESSED:
      const audits = action.audits.reduce((obj, audit) => {
        obj[audit.id] = audit;
        return obj;
      }, {});
      return {
        ...state,
        byIds: action.audits.map(audit => audit.id),
        audits: audits,
        loaded: true,
        loading: false
      };

    case LOAD_AUDITS_FAILED:
      return {
        ...state,
        byIds: [],
        audits: {},
        error: action.error,
        loaded: true,
        loading: false
      };

    default:
      return state;
  }
}

export function auditsLoading() {
  return { type: LOAD_AUDITS };
}

export function loadAudits(audits) {
  return { type: LOAD_AUDITS_SUCCESSED, audits };
}

export function getAudits() {
  return (dispatch, getState) => {
    if (!getState()["auditStore"]["loaded"]) {
      dispatch(auditsLoading());
      return fetch(LOAD_AUDITS_URL)
        .then(response => response.json())
        .then(audits => dispatch(loadAudits(audits)));
    }
  };
}
