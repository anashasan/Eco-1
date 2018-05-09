const createOptionsBySelectList = (selectorId, models) => {
  /** @type {HTMLSelectElement} */
  const sel = document.getElementById(selectorId);
  sel.options.length = 1;
  const fragment = document.createDocumentFragment();

  for (let model of models) {
    const opt = document.createElement("option");
    opt.innerHTML = model.text;
    opt.disabled = model.disabled;
    opt.value = model.value;
    opt.selected = model.selected;
    fragment.appendChild(opt);
  }
  sel.appendChild(fragment);
};

const getBranchDropdownById = id => {
  const stationData = {};
  const headers = new Headers();
  headers.append("Accept", "application/json");
  headers.append("Content-Type", "application/json");
  fetch(`/Company/GetBranchByCompanyId/id/${id}`, {
    credentials: "same-origin",
    headers: headers,
    method: "GET"
  })
    .then(function(response) {
      if (response.ok) {
        response
          .json()
          .then(data => {
            createOptionsBySelectList("BranchId", data);
          })
          .catch(ex => {});
      }
    })
    .catch(ex => {});
};
