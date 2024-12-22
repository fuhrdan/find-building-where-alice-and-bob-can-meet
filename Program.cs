//*****************************************************************************
//** 2940. Find Building Where Alice and Bob Can Meet               leetcode **
//*****************************************************************************
//** This solution does NOT work in it's current form.  Two changes will     **
//** make it work.  But you have to know where to make those changes. -Dan   **
//*****************************************************************************

/**
 * Note: The returned array must be malloced, assume caller calls free().
 */
typedef struct {
    int* tree;
    int size;
} BinaryIndexedTree;

// Function to create a new Binary Indexed Tree
BinaryIndexedTree* createBIT(int size) {
    BinaryIndexedTree* bit = (BinaryIndexedTree*)malloc(sizeof(BinaryIndexedTree));
    if (!bit) {
        fprintf(stderr, "Memory allocation failed for BinaryIndexedTree.\n");
        exit(1);
    }
    bit->size = size;
    bit->tree = (int*)malloc((size + 1) * sizeof(int));  // Ensure sufficient space for the tree
    if (!bit->tree) {
        fprintf(stderr, "Memory allocation failed for Binary Indexed Tree data.\n");
        exit(1);
    }

    // Initialize tree with INT_MAX (equivalent to infinity)
    for (int i = 0; i <= size; ++i) {
        bit->tree[i] = INT_MAX;
    }
    return bit;
}

// Update the Binary Indexed Tree (BIT) to store the index of the tallest building so far
void updateBIT(BinaryIndexedTree* bit, int x, int val) {
    while (x <= bit->size) {
        if (bit->tree[x] > val) {
            bit->tree[x] = val;
        }
        x += x & -x;  // move to the next index
    }
}

// Query the Binary Indexed Tree for the leftmost building within the range
int queryBIT(BinaryIndexedTree* bit, int x) {
    int minimum = INT_MAX;
    while (x > 0) {
        minimum = (minimum < bit->tree[x]) ? minimum : bit->tree[x];
        x -= x & -x;  // move to the previous index
    }
    return minimum == INT_MAX ? -1 : minimum;
}

int* leftmostBuildingQueries(int* heights, int heightsSize, int** queries, int queriesSize, int* queriesColSize, int* returnSize) {
    int* results = (int*)malloc(queriesSize * sizeof(int)); 
    *returnSize = queriesSize;  
    
    for (int i = 0; i < queriesSize; ++i) {
        int leftBound = queries[i][0];
        int rightBound = queries[i][1];
        
        // Create a new BIT for each query
        BinaryIndexedTree* bit = createBIT(heightsSize);
        
        // Update BIT with buildings within the range [leftBound, rightBound]
        for (int j = leftBound; j <= rightBound; ++j) {
            updateBIT(bit, j + 1, heights[j]);  // 1-based indexing
        }

        // Now query the BIT for the leftmost building
        int leftmost = queryBIT(bit, rightBound + 1); // Query for the minimum index in the range
        results[i] = leftmost;
        
        // Free the BIT after each query
        free(bit->tree);
        free(bit);
    }
    
    return results;
}